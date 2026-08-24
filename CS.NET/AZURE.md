### AZ CMD using:

```powershell
az communication email domain sender-username CREATE --domain-name
                                                     --email-service-name
                                                     --name --sender-username
                                                     --resource-group
                                                     [--display-name]
                                                     [--username]

### Subscription-Name  -  063fbec7-1f03-462e-9a05-c759761b0111  -  Default Directory

.\az.cmd login --tenant 063fbec7-1f03-462e-9a05-c759761b0111  (see TenantID on AZ Portal)

.\az.cmd communication email domain sender-username SHOW --domain-name "test.com" --email-service-name "EmailCommServicesRes-0813" --resource-group "ResourceGroup-0813" --name "DoNotReply"

.\az.cmd communication email domain sender-username CREATE --domain-name "test.com" --email-service-name "EmailCommServicesRes-0813" --resource-group "ResourceGroup-0813" --sender-username "DisplayingSenderName" --username "DisplayingSenderName" --display-name "Displaying Sender Name"

.\az.cmd communication email SEND --sender "DoNotReply@test.com" --subject "Sent from CLI" --to "abcdef-01@testing.com" "abcdef-02@testing.com" --text "Mail sent using Azure CLI interface."

# ### Username: <Azure Communication Service name>.<App registration client ID>.<Entra tenant ID>

```