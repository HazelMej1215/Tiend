package com.example.tiendaapp.network

import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.POST

interface TiendaApiService {
    @GET("api/Productos")
    suspend fun getProductos(): List<ApiProduct>

    @POST("api/Compras")
    suspend fun comprar(@Body req: CompraRequest): CompraResponse
}
