# Thawmadoce
 
*Th*e *aw*esome *ma*rk*do*wn *ce*ntrifuge

A project that puts a markdown editor at its center and lets you plug shit around it.


## Licensing

Developed by Frank-Leonardo Quednau ([realfiction.net](http://realfiction.net)).
Licensed under the [GPL](http://www.gnu.org/copyleft/gpl.html)

# Current platform

.NET 4, WPF

# Dependencies

* Caliburn.Micro
* StructureMap
* Membus
* Rx
* Markdownsharp

## Where's the packages folder?

This project is checked in without the actual binaries of the required Nuget-packages. This
kind of workflow is enabled through NugetPowerTools and is used as described here:
[The easy way to set up NuGet to restore packages](http://blog.davidebbo.com/2011/08/easy-way-to-set-up-nuget-to-restore.html)
