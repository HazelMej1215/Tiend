package com.example.tiendaapp.network

import com.google.gson.annotations.SerializedName

data class ApiProduct(
    @SerializedName("idProducto") val idProducto: Int,
    @SerializedName("nombre") val nombre: String,
    @SerializedName("precio") val precio: Double,
    @SerializedName("stock") val stock: Int,
    @SerializedName("sku") val sku: String? = null,
    @SerializedName("idCategoria") val idCategoria: Int? = null,
    @SerializedName("idEstado") val idEstado: Int? = null

)

data class CompraRequest(
    @SerializedName("idUsuario") val idUsuario: Int? = null,
    @SerializedName("items") val items: List<ItemCompra>
)

data class ItemCompra(
    @SerializedName("idProducto") val idProducto: Int,
    @SerializedName("cantidad") val cantidad: Int
)

data class CompraResponse(
    @SerializedName("idVenta") val idVenta: Int,
    @SerializedName("total") val total: Double,
    @SerializedName("estatus") val estatus: String
)
