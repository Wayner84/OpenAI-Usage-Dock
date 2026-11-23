from pathlib import Path
from shutil import copytree, copy2
import sys
import xml.etree.ElementTree as ET

root = Path(__file__).resolve().parents[1]
version = sys.argv[1] if len(sys.argv) == 2 else '1.0.0.0'
manifest = root / 'packaging' / 'AppxManifest.xml'
actual = ET.parse(manifest).find('{http://schemas.microsoft.com/appx/manifest/foundation/windows10}Identity').attrib['Version']
if actual != version:
    raise SystemExit(f'Manifest version {actual} differs from staged version {version}')
source = root / f'publish-{version}'
target = root / 'artifacts' / f'package-{version}'
if target.exists():
    raise SystemExit(f'Refusing to overwrite existing stage: {target}')
copytree(source, target)
copy2(root / 'packaging' / 'AppxManifest.xml', target / 'AppxManifest.xml')
print(target)
