package com.boostmytool.store.models;

import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotEmpty;
public class ProductView {

	@NotEmpty(message = "The name is required")
	private String name;
	@NotEmpty(message = "The name is required")
	private String category;
	@Min(0)
	private int quantity;
	@Min(0)
	private double price;
	private String information;
	@NotEmpty(message = "The name is required")
	private String image;
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public String getCategory() {
		return category;
	}
	public void setCategory(String category) {
		this.category = category;
	}
	public int getQuantity() {
		return quantity;
	}
	public void setQuantity(int quantity) {
		this.quantity = quantity;
	}
	public double getPrice() {
		return price;
	}
	public void setPrice(double price) {
		this.price = price;
	}
	public String getInformation() {
		return information;
	}
	public void setInformation(String information) {
		this.information = information;
	}
	public String getImage() {
		return image;
	}
	public void setImage(String image) {
		this.image = image;
	}
	
}
