# terraform {
#   backend "azurerm" {
#     resource_group_name  = "Reda"
#     storage_account_name = "redastorageaccnt1234"
#     container_name       = "content"
#     key                  = "terraform.tfstate"
#   }
# }

terraform {
  backend "azurerm" {
    resource_group_name   = "Reda"
    storage_account_name  = "redastorageaccnt1234"
    container_name        = "content"
    key                   = "terraform.tfstate"

    
  }

   
}





