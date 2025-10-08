
provider "azurerm" {
  features {}

  subscription_id = var.subscription_id
  tenant_id       = var.tenant_id
  client_id       = var.client_id
  client_secret   = var.client_secret

  use_cli = false 
    
}



data "azurerm_resource_group" "rg" {
    name = "Reda"
}

resource "azurerm_virtual_network" "vnet" {
  name                = "CenterVNet"
  address_space       = ["10.0.0.0/16"]
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
}


resource "azurerm_subnet" "subnet" {
  name                 = "CenterSubnet"
  resource_group_name  = data.azurerm_resource_group.rg.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = ["10.0.1.0/24"]

}


resource "azurerm_network_security_group" "nsg" {
    name                = "CenterNSG"
    location            = data.azurerm_resource_group.rg.location
    resource_group_name = data.azurerm_resource_group.rg.name

    security_rule {
        name                       = "allow_ssh"
        priority                   = 100
        direction                  = "Inbound"
        access                     = "Allow"
        protocol                   = "Tcp"
        source_port_range          = "*"
        destination_port_range     = "22"
        source_address_prefix      = "*"
        destination_address_prefix = "*"
    }
    security_rule {
        name                       = "allow_http"
        priority                   = 101
        direction                  = "Inbound"
        access                     = "Allow"
        protocol                   = "Tcp"
        source_port_range          = "*"
        destination_port_range     = "80"
        source_address_prefix      = "*"
        destination_address_prefix = "*"
    }
    security_rule {
        name                       = "allow_https"
        priority                   = 102
        direction                  = "Inbound"
        access                     = "Allow"
        protocol                   = "Tcp"
        source_port_range          = "*"
        destination_port_range     = "443"          
        source_address_prefix      = "*"
        destination_address_prefix = "*"
    }
}

resource "azurerm_subnet_network_security_group_association" "subnet_nsg_association" {
    subnet_id                 = azurerm_subnet.subnet.id
    network_security_group_id = azurerm_network_security_group.nsg.id
}

resource "azurerm_network_interface" "nic" {
  name                = "CenterNIC"
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name

  ip_configuration {
    name                          = "internal"
    subnet_id                     = azurerm_subnet.subnet.id
    private_ip_address_allocation = "Dynamic"
    public_ip_address_id          = azurerm_public_ip.public_ip.id
  }
  
}

resource "azurerm_public_ip" "public_ip" {
    name                = "CenterPublicIP"
    location            = data.azurerm_resource_group.rg.location
    resource_group_name = data.azurerm_resource_group.rg.name
    allocation_method   = "Static"

}
resource "azurerm_linux_virtual_machine" "vm" {
  name                = "CenterVM"
  resource_group_name = data.azurerm_resource_group.rg.name
  location            = data.azurerm_resource_group.rg.location
  size                = "Standard_B1s"
  admin_username      = "adminuser"
  network_interface_ids = [
    azurerm_network_interface.nic.id,
  ]

  admin_ssh_key {
    username   = "adminuser"
    public_key = file("~/.ssh/id_rsa.pub")
  }

  os_disk {
    caching              = "ReadWrite"
    storage_account_type = "Standard_LRS"
  }

  source_image_reference {
    publisher = "Canonical"
    offer     = "UbuntuServer"
    sku       = "18.04-LTS"
    version   = "latest"
  }
  
}

resource "azurerm_virtual_machine_extension" "add_new_ssh_key" {
  name                 = "AddNewSSHKey"
  virtual_machine_id   = azurerm_linux_virtual_machine.vm.id
  publisher            = "Microsoft.Azure.Extensions"
  type                 = "CustomScript"
  type_handler_version = "2.1"

  settings = <<SETTINGS
    {
      "commandToExecute": "echo 'ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQCxLMM4loasoei2nmVu5YnFqYW7mcNypzseGO01VVTCy+QP9351H4sE3bbCPrgtLUSw1CuoMBVA76YYCBztknuBt8CvTutiTDOc10Hm/zyTzRlJYfVdjTWmXtgPk4ZbZxz0d4J5X7t8jANGXxxXV4CCz35ITavG5vPeVJ5feD/meGzHXqfeWixH8NZdAeQujmqLcIEUoSFHbvTIZRyorD/84AdQkY/Vl/gBkIdOxHn43oXwXXgYj09jDcwIumkdtxykvBUNvCijCB6WfAvTLX7j+2PbDMBvLEIv33FEYJRG3MLq+iTODEMByM6PySWUlV/fKA2wrC2QVSa/ST3t4+Zx newkey@terra' >> /home/adminuser/.ssh/authorized_keys"
    }
  SETTINGS
}



output "public_ip" {
  value = azurerm_public_ip.public_ip.ip_address
}


