#!/usr/bin/env python

import sys


BACKUP_FILE = 'shortcut_swap.bk'


def log(line, end='\n'):
    sys.stdout.write(line if (end == '') else (line + end))


class RevertDump:
    def __init__(self):
        self.backup = dict()
