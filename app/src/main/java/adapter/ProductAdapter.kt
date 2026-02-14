package com.example.tiendaapp.adapter

import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.example.tiendaapp.data.Product
import com.example.tiendaapp.databinding.ItemProductBinding

class ProductAdapter(
    private var items: List<Product>,
    private val onBuyClick: (Product) -> Unit
) : RecyclerView.Adapter<ProductAdapter.VH>() {

    fun update(newItems: List<Product>) {
        items = newItems
        notifyDataSetChanged()
    }

    inner class VH(val b: ItemProductBinding) : RecyclerView.ViewHolder(b.root)

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): VH {
        val b = ItemProductBinding.inflate(LayoutInflater.from(parent.context), parent, false)
        return VH(b)
    }

    override fun onBindViewHolder(holder: VH, position: Int) {
        val p = items[position]

        holder.b.txtName.text = "${p.name} (ID: ${p.id})"
        holder.b.txtCategory.text = "Categoría: ${p.category}"
        holder.b.txtPrice.text = "Precio: $${"%.2f".format(p.price)}"
        holder.b.txtStock.text = "Stock: ${p.stock} | Estado: ${p.status}"

        val canBuy = p.status == "ACTIVE" && p.stock > 0
        holder.b.btnBuy.isEnabled = canBuy
        holder.b.btnBuy.text = if (canBuy) "Comprar" else "No disponible"

        holder.b.btnBuy.setOnClickListener { onBuyClick(p) }
    }

    override fun getItemCount(): Int = items.size
}
