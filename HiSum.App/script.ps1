Get-ChildItem .\* -include ('*.css', '*.js','*.json','*.ts','*.html','*.dll') -Recurse|where {$_ -notmatch ('bin') -and $_ -notmatch('obj')}|Write-Zip -OutputPath .\app.zip
Rename-Item -Path .\app.zip -newName "app.nw"
C:\node_webkit\nw.exe .\app.nw