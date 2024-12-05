package com.boostmytool.store.controllers;

import java.util.List;

import javax.naming.Binding;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.validation.BindingResult;
import org.springframework.validation.FieldError;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.ModelAttribute;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

import com.boostmytool.store.models.ProductView;
import com.boostmytool.store.models.product;
import com.boostmytool.store.services.ProductsRepository;

import jakarta.validation.Valid;


@Controller
@RequestMapping("/products")
public class ProductsController {

	@Autowired
	private ProductsRepository resp;
	@GetMapping({"","/"})	
	public String showProductList(Model model)
	{
		List<product> products =resp.findAll();
		model.addAttribute("products", products);
		return "products/index";
	}
	@GetMapping("/create")
	public String showCreateProductPage(Model model)
	{
		ProductView productView =new ProductView();
		model.addAttribute("productView", productView);
		return "products/CreateProduct";
	}
	@PostMapping("/create")
	public String CreateProduct(@Valid @ModelAttribute ProductView productView, BindingResult result)
	{
		
		if(result.hasErrors())
		{
			return "products/CreateProduct";
		}
		product product = new product();
		product.setName(productView.getName());
		product.setCategory(productView.getCategory());
		product.setImage(productView.getImage());
		product.setInformation(productView.getInformation());
		product.setPrice(productView.getPrice());
		product.setQuantity(productView.getQuantity());
		resp.save(product);
		return "redirect:/products";
	}
	@GetMapping("/delete")
	public String deleteProduct(@RequestParam int id)
	{
		try {
			product product = resp.findById(id).get();
			resp.delete(product);
			
		} catch (Exception e) {
			// TODO: handle exception
		}
		return "redirect:/products";
	}
	@GetMapping("/show")
	public String showProduct(@RequestParam int id,Model model)
	{
		try {
			product product = resp.findById(id).get();
			model.addAttribute("${products}", product);
			return "products/ShowProduct";
			
		} catch (Exception e) {
			// TODO: handle exception
		}
		return "redirect:/products";
	}
	
}
