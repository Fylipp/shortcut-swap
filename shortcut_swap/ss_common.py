#!/usr/bin/env python

from win32com.client import Dispatch

import sys
import os


BACKUP_FILE = 'shortcut_swap.bk'


def log(line, end='\n'):
    sys.stdout.write(line if (end == '') else (line + end))


def get_shortcut_info(shortcut_location):
    shell = Dispatch('WScript.Shell')
    shortcut = shell.CreateShortcut(shortcut_location)
    return ShortcutInfo(shortcut.TargetPath, shortcut.WorkingDirectory, shortcut.Arguments)


def set_shortcut_info(shortcut_location, info, icon_override=None):
    shell = Dispatch('WScript.Shell')
    shortcut = shell.CreateShortcut(shortcut_location)
    shortcut.TargetPath = info.target_path
    shortcut.WorkingDirectory = info.working_directory
    shortcut.Arguments = info.arguments
    shortcut.IconLocation = target_path if icon_override is None else icon_override
    shortcut.Save()


class RevertDump:
    def __init__(self):
        self.backup = dict()


class ShortcutInfo:
    def __init__(self, target_path, working_directory=u'.\\', arguments=u''):
        self.target_path = target_path
        self.working_directory = working_directory
        self.arguments = arguments
