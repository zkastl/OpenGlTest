function CompareHashValue
{
    $file = $args[0]
    $compare = $args[1]

    #$file = '.\raytracer_master.bundle'
    $hashval = (Get-FileHash $file).Hash
    write-host $hashval
}

$input = Read-Host "Please enter the drive letter for your optical drive (e.g: D, E):"
$opticaldrive = [System.IO.DriveInfo]::GetDrives() | where Name -Match $input
$bundles =  [System.IO.DirectoryInfo]::new($opticaldrive).GetFiles() | where Name -Match ".bundle"
$copylocation = ($env:USERPROFILE + '\Desktop\' + $bundles.Name)
write-host "Copying to " + $copylocation
cp $bundles.FullName $copylocation
CompareHashValue $bundles.FullName