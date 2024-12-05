package com.boostmytool.store.services;

import org.springframework.data.jpa.repository.JpaRepository;

import com.boostmytool.store.models.product;

public interface ProductsRepository extends JpaRepository<product, Integer> {

}
