package com.example.tiendaapp

import android.os.Bundle
import android.widget.Toast
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.tiendaapp.adapter.ProductAdapter
import com.example.tiendaapp.data.Product
import com.example.tiendaapp.databinding.ActivityMainBinding

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

        loadFakeProducts()
    }

    private fun loadFakeProducts() {
        val fake = listOf(
            Product(1, "Teclado mecánico", "Periféricos", 899.99, 10, "ACTIVE"),
            Product(2, "SSD 1TB", "Almacenamiento", 1299.50, 4, "ACTIVE"),
            Product(3, "GPU RTX", "Componentes", 9999.99, 0, "ACTIVE"),
            Product(4, "Mouse gamer", "Periféricos", 499.00, 7, "INACTIVE")
        )
        adapter.update(fake)
    }

    private fun showBuyDialog(product: Product) {
        val quantities = arrayOf("1", "2", "3")

        AlertDialog.Builder(this)
            .setTitle("Comprar ${product.name}")
            .setItems(quantities) { _, which ->
                val qty = quantities[which].toInt()
                Toast.makeText(this, "Compra simulada: ${product.name} x$qty", Toast.LENGTH_SHORT).show()
            }
            .setNegativeButton("Cancelar", null)
            .show()
    }
}
