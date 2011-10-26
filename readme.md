# Thawmadoce
 
__Th__e __aw__esome __ma__rk__do__wn __ce__ntrifuge

(Please speak the 'c' like the 'cc' in 'Capuccino')

A project that puts a markdown editor at its center and lets you plug shit around it.
V1 will be insanely crude and will not stand against the previous sentence.
I wouldn't be the first creating vapourware and please consider that Duke Nukem actually got a release.


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

Icons have been used by

* [the oxygen team](http://www.iconarchive.com/artist/oxygen-icons.org.html) (associated with KDE)
* [deleket](http://www.iconarchive.com/artist/deleket.html)

## Where's the packages folder?

This project is checked in without the actual binaries of the required Nuget-packages. This
kind of workflow is enabled through NugetPowerTools and is used as described here:
[The easy way to set up NuGet to restore packages](http://blog.davidebbo.com/2011/08/easy-way-to-set-up-nuget-to-restore.html)