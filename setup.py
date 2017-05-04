#!/usr/bin/env python

from setuptools import setup, find_packages

setup(
    name = "shortcut_swap",
    version = "0.1",
    author = "Philipp Ploder",
    url = "https://github.com/Fylipp/shortcut-swap",
    packages = find_packages(),
    long_description = """\
    Replaces all shortcuts in a folder hierarchy with a custom one. Can be reversed.
    """,
    classifiers = [
        "License :: License :: OSI Approved :: MIT License",
        "Programming Language :: Python",
        "Development Status :: Development Status :: 3 - Alpha",
        "Intended Audience :: Intended Audience :: End Users/Desktop",
        "Topic :: Utilities"
    ],
    keywords = "tool",
    license = "MIT",
    install_requires = [
        "setuptools"
    ]
)

