Clear-Host

# hangfire-jobmanagement
helm upgrade --install hangfire-jobmanagement charts/hangfire-jobmanagement `
  --namespace app-hangfire-jobmanagement `
  --create-namespace `
  --set image.tag=latest
