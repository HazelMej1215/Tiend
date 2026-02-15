package com.example.tiendaapp

import android.os.Bundle
import android.widget.Toast
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.lifecycleScope
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.tiendaapp.adapter.ProductAdapter
import com.example.tiendaapp.data.Product
import com.example.tiendaapp.databinding.ActivityMainBinding
import com.example.tiendaapp.network.ApiClient
import com.example.tiendaapp.network.CompraRequest
import com.example.tiendaapp.network.ItemCompra
import kotlinx.coroutines.launch

class MainActivity : AppCompatActivity() {

    private lateinit var b: ActivityMainBinding
    private lateinit var adapter: ProductAdapter

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        b = ActivityMainBinding.inflate(layoutInflater)
        setContentView(b.root)

        adapter = ProductAdapter(emptyList()) { product ->
            showBuyDialog(product)
        }

        b.rvProducts.layoutManager = LinearLayoutManager(this)
        b.rvProducts.adapter = adapter

        loadFromApi()
    }

    private fun loadFromApi() {
        lifecycleScope.launch {
            try {
                val apiProducts = ApiClient.service.getProductos()

                val mapped = apiProducts.map { ap ->
                    Product(
                        id = ap.idProducto,
                        name = ap.nombre,
                        category = "${ap.idCategoria ?: 0}",
                        price = ap.precio,
                        stock = ap.stock,
                        status = if ((ap.idEstado ?: 1) == 1) "ACTIVE" else "INACTIVE"
                    )
                }

                adapter.update(mapped)

            } catch (e: Exception) {
                Toast.makeText(
                    this@MainActivity,
                    "Error API: ${e.message}",
                    Toast.LENGTH_LONG
                ).show()
            }
        }
    }

    private fun showBuyDialog(product: Product) {
        val quantities = arrayOf("1", "2", "3")

        AlertDialog.Builder(this)
            .setTitle("Comprar ${product.name}")
            .setItems(quantities) { _, which ->
                val qty = quantities[which].toInt()

                lifecycleScope.launch {
                    try {
                        val resp = ApiClient.service.comprar(
                            CompraRequest(
                                idUsuario = null,
                                items = listOf(
                                    ItemCompra(product.id, qty)
                                )
                            )
                        )

                        Toast.makeText(
                            this@MainActivity,
                            "Compra OK #${resp.idVenta} Total: ${resp.total}",
                            Toast.LENGTH_LONG
                        ).show()

                        loadFromApi()

                    } catch (e: Exception) {
                        Toast.makeText(
                            this@MainActivity,
                            "Compra error: ${e.message}",
                            Toast.LENGTH_LONG
                        ).show()
                    }
                }
            }
            .setNegativeButton("Cancelar", null)
            .show()
    }
}
