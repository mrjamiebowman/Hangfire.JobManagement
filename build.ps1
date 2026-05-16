Clear-Host

# docker build
$VERSION = 'local'

docker build --no-cache -f "test\Hangfire.JobManagement.Test.SampleServer\Dockerfile" `
    --label "company=mrjamiebowman" `
    -t mrjamiebowman/hangfire-jobmanagement:$VERSION .

# display built images
docker images | findstr mrjamiebowman/hangfire-jobmanagement