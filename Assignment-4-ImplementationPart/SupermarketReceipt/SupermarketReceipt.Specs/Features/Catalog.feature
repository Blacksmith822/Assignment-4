Feature: Catalog

	Scenario: Add products to catalog
		Given an empty catalog
		When these products are added to the catalog:
		  | Product    | Price |
		  | Rice       | 2.49  |
		  | Milk       | 1.29  |
		  | Toothbrush | 0.99  |
		Then the catalog should have 3 products
		And contain Rice at 2.49
		And contain Milk at 1.29
		And contain Toothbrush at 0.99
		And there should be no exception
		
	Scenario: Cannot add a product that is already in the catalog
		Given an empty catalog
		When these products are added to the catalog:
		  | Product    | Price |
		  | Rice       | 2.49  |
		  | Milk       | 1.29  |
		  | Toothbrush | 0.99  |
    	And these products are added to the catalog:
	      | Product | Price |
	      | Rice    | 4.49  |
		Then the catalog should have 3 products
		And contain Rice at 2.49
		And there should be an exception saying "Cannot add a product that is already in the catalog."
		
	Scenario: Add and remove products to and from catalog
		Given an empty catalog
		When these products are added to the catalog:
		  | Product    | Price |
		  | Rice       | 2.49  |
		  | Milk       | 1.29  |
		  | Toothbrush | 0.99  |
    	And these products are removed from the catalog:
	      | Product    |
	      | Rice       |
	      | Toothbrush |
		Then the catalog should have 1 products
		And contain Milk at 1.29
		And there should be no exception
		
	Scenario: Cannot remove a product that is not in the catalog
		Given an empty catalog
		When these products are removed from the catalog:
			| Product    |
			| Rice       |
		Then the catalog should have 0 products
		And there should be an exception saying "Cannot remove a product that is not in the catalog."
		
	Scenario: Apply simple percent discount to a product in the catalog
		Given an empty catalog
		When these products are added to the catalog:
		  | Product    | Price |
		  | Rice       | 2.49  |
		  | Milk       | 1.29  |
		  | Toothbrush | 0.99  |
		And a simple 10 percent discount is applied to Milk
		Then the catalog should have 3 products
		And contain Milk with 10 percent discount
		And there should be no exception
		
	Scenario: Apply a many for the price of a few deal
		Given an empty catalog
		When these products are added to the catalog:
		  | Product    | Price |
		  | Rice       | 2.49  |
		  | Milk       | 1.29  |
		  | Toothbrush | 0.99  |
		And a 2 for the price of 1 deal is applied to Milk
		And a 3 for the price of 2 deal is applied to Toothbrush
		Then the catalog should have 3 products
		And contain Milk with a 2 for the price of 1 deal
		And contain Toothbrush with a 3 for the price of 2 deal
		And there should be no exception
		
	Scenario: Applying a deal to the product overrides any existing deal on that product
		Given an empty catalog
		When these products are added to the catalog:
		  | Product    | Price |
		  | Rice       | 2.49  |
		  | Milk       | 1.29  |
		  | Toothbrush | 0.99  |
		And a simple 50 percent discount is applied to Rice
		And a 3 for the price of 1 deal is applied to Rice
		Then the catalog should have 3 products
		And contain Rice with a 3 for the price of 1 deal
		