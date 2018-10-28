# Condemned-DDS-Converter

A basic command-line program for stripping and attaching headers to Condemned: Criminal Origin's DDS files.

For mass converting files, you can use this script:
```
for /r %%v in (*.dds) do (
Condemned-DDS-converter.exe  %%v
)
```
