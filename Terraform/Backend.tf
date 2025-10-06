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

    subscription_id       = var.subscription_id
    tenant_id             = var.tenant_id
    client_id             = var.client_id
    client_secret         = var.client_secret
  }
}




