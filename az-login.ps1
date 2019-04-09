# Before you begin - make sure you're logged in to the azure CLI
az login

# Ensure you choose the correct azure subscription if you have more than one 
$SUBSCRIPTION_NAME = "My Azure Subscription"

Write-Verbose "Set the default Azure subscription" -Verbose
az account set --subscription "$SUBSCRIPTION_NAME"

$SUBSCRIPTION_ID = $(az account show --query id -o tsv)
Write-Verbose "SUBSCRIPTION_ID: $SUBSCRIPTION_ID" -Verbose

$SUBSCRIPTION_NAME = $(az account show --query name -o tsv)
Write-Verbose "SUBSCRIPTION_NAME: $SUBSCRIPTION_NAME" -Verbose

$USER_NAME = $(az account show --query user.name -o tsv)
Write-Verbose "Service Principal Name or ID: $USER_NAME" -Verbose

$TENANT_ID = "52034371-b33a-42e9-85c2-78ec57d3d8e0"
Write-Verbose "TENANT_ID: $TENANT_ID" -Verbose

$SCRIPT_DIRECTORY = ($pwd).path
Write-Verbose "SCRIPT_DIRECTORY: $SCRIPT_DIRECTORY" -Verbose