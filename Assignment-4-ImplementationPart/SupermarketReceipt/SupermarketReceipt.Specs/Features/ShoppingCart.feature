Feature: ShoppingCart

	Scenario: Add an item to a cart
		Given the catalog contains:
		  | Product    | Price |
		  | Toothbrush | 0.99  |
		And an empty shopping cart
		When Toothbrush is added to the cart
		Then there should should be 1 items in the cart
		And there should not be an exception
	
	Scenario: Add multiple items to a cart
		Given the catalog contains:
		  | Product    | Price |
		  | Rice       | 2.49  |
		  | Milk       | 1.29  |
		  | Toothbrush | 0.99  |
		And an empty shopping cart
		When Rice is added to the cart
		And Milk is added to the cart
		And Toothbrush is added to the cart
		Then there should should be 3 items in the cart
		And there should not be an exception
		
	Scenario: Cannot add an item to the cart that is not in the catalog
		Given the catalog contains:
		  | Product    | Price |
		  | Milk       | 1.29  |
		  | Toothbrush | 0.99  |
		And an empty shopping cart
		When Rice is added to the cart
		Then there should been an exception saying "Cannot add an item to the cart that is not in the catalog."
		
	Scenario: Remove an item from a cart
		Given the catalog contains:
		  | Product    | Price |
		  | Toothbrush | 0.99  |
		And an empty shopping cart
		When Toothbrush is added to the cart
		And Toothbrush is removed from the cart
		Then there should should be 0 items in the cart
		And there should not be an exception
		
	Scenario: Cannot remove an item that is not in the cart
		Given the catalog contains:
		  | Product    | Price |
		  | Milk       | 1.29  |
		  | Toothbrush | 0.99  |
		And an empty shopping cart
		When Milk is added to the cart
		And Toothbrush is removed from the cart
		Then there should been an exception saying "Cannot remove an item that is not in the cart."
		
	Scenario: Add and remove multiple items from a cart
		Given the catalog contains:
		  | Product    | Price |
		  | Rice       | 2.49  |
		  | Milk       | 1.29  |
		  | Toothbrush | 0.99  |
		And an empty shopping cart
		When Rice is added to the cart
		And Milk is added to the cart
		And Toothbrush is added to the cart
		And Toothbrush is removed from the cart
		And Rice is removed from the cart
		Then there should should be 1 items in the cart
		And there should not be an exception

	Scenario: Receipt for cart with one item
		Given the catalog contains:
		  | Product    | Price |
		  | Toothbrush | 0.99  |
		And an empty shopping cart
		When Toothbrush is added to the cart
		And a receipt is generated
		Then the receipt should have:
		  | Product    | Quantity | Amount |
		  | Toothbrush |    1     | 0.99   |
		And the receipt's total amount should be 0.99
		And there should not be an exception
		
	Scenario: Receipt for cart with multiple items
		Given the catalog contains:
		  | Product    | Price |
		  | Rice       | 2.49  |
		  | Milk       | 1.29  |
		  | Toothbrush | 0.99  |
		And an empty shopping cart
		When Rice is added to the cart
		And Milk is added to the cart
		And Toothbrush is added to the cart
		And a receipt is generated
		Then the receipt should have:
		  | Product    | Quantity | Amount |
		  | Rice       |    1     | 2.49   |
		  | Milk       |    1     | 1.29   |
		  | Toothbrush |    1     | 0.99   |
		And the receipt's total amount should be 4.77
		And there should not be an exception
		
	Scenario: Receipt for cart with multiple of the same items
		Given the catalog contains:
		  | Product    | Price |
		  | Rice       | 2.49  |
		  | Milk       | 1.29  |
		  | Toothbrush | 0.99  |
		And an empty shopping cart
		When Rice is added to the cart
		And Milk is added to the cart
		And Milk is added to the cart
		And Toothbrush is added to the cart
		And Toothbrush is added to the cart
		And Toothbrush is added to the cart
		And a receipt is generated
		Then the receipt should have:
		  | Product    | Quantity | Amount |
		  | Rice       |    1     | 2.49   |
		  | Milk       |    2     | 2.58   |
		  | Toothbrush |    3     | 2.97   |
		And the receipt's total amount should be 8.04
		And there should not be an exception
		
	Scenario: Receipt for cart with an item with a simple percent discount
		Given the catalog contains:
		  | Product    | Price |
		  | Milk       | 1.29  |
		  | Rice       | 2.49  |
    	And Milk has a 10 percent discount
    	And an empty shopping cart
    	When Milk is added to the cart
    	And Rice is added to the cart
    	And a receipt is generated
		Then the receipt should have:
		  | Product | Quantity | Amount |
		  | Milk    |    1     | 1.161  |
		  | Rice    |    1     | 2.49   |
    	And the receipt's total amount should be 3.651
    	And there should not be an exception
    	
	Scenario: Email receipt for cart with an item with a simple percent discount
		Given the catalog contains:
		  | Product | Price |
		  | Milk    | 1.29  |
		  | Rice    | 2.49  |
		And Milk has a 10 percent discount
		And an empty shopping cart
		When Milk is added to the cart
		And Rice is added to the cart
		And a receipt is generated
		And the receipt is emailed to customer@email.com
		Then the receipt should have:
		  | Product | Quantity | Amount |
		  | Milk    |    1     | 1.161  |
		  | Rice    |    1     | 2.49   |
		And the receipt's total amount should be 3.651
		And the receipt should have been emailed
		And there should not be an exception