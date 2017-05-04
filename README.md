# shortcut-swap
Swap all shortcuts in any given folder hierarchy with a custom shortcut. Can be reversed.

## Usage
### Swapping
`python -m shortcut_swap swap <root> <link> [depth_limit] [verbose]`

Required:
- root: The root of the folder hierarchy at which the script should start
- link: The custom shortcut to change all `.lnk` files to

Optional:
- depth_limit: The maximum hierarchy depth, defaults to 12
- verbose: Whether the hierarchy progression should be logged (increases time it takes to execute by a lot), defaults to `False`

## Reverting
`python -m shortcut_swap revert <root> [verbose]`

Required:
- root: The root of the folder hierarchy that is to be restored

Optional:
- verbose: Whether the hierarchy progression should be logged (increases time it takes to execute by a lot), defaults to `False`

## Installation
To install you simply need to do `pip install git+https://github.com/Fylipp/shortcut-swap.git`.

## License
**shortcut-swap** is published under the MIT License. The license can be found in the `LICENSE` file.

