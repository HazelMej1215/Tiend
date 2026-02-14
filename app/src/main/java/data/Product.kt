package com.example.tiendaapp.data

data class Product(
    val id: Int,
    val name: String,
    val category: String,
    val price: Double,
    val stock: Int,
    val status: String // "ACTIVE" / "INACTIVE"
)
