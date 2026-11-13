Clear-Host

# template
helm template hangfire-jobmanagement charts/hangfire-jobmanagement `
  --namespace app-hangfire-jobmanagement `
  --set image.tag=latest
