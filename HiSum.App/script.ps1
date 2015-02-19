Import-Module Pscx

If (Test-Path .\app.nw) {
   Remove-Item .\app.nw
}
Get-ChildItem .\* -include ('*.css', '*.js','*.json','*.ts','*.html','*.dll') -Recurse|where {$_ -notmatch ('bin') -and $_ -notmatch('obj')}|Write-Zip -OutputPath .\app.zip
Rename-Item -Path .\app.zip -newName "app.nw"
.\nw.exe .\app.nw