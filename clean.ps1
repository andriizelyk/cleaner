# Define the root folder to start the search
$rootFolder = "C:\Path\To\Your\Folder"

# Get all .sln files in the root folder and its subfolders
$slnFiles = Get-ChildItem -Path $rootFolder -Recurse -Filter *.sln

# Iterate through each .sln file and run 'dotnet clean'
foreach ($slnFile in $slnFiles) {
    $fullPath = $slnFile.FullName
    Write-Host "Cleaning solution: $fullPath"
    dotnet clean $fullPath
}

Write-Host "All solutions cleaned successfully."
