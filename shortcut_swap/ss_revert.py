#!/usr/bin/env python

from ss_common import *

import os
import pickle


def revert(root, verbose=False):
    if not os.path.exists(root):
            log("The provided path does not exist")
            return

    bkPath = os.path.join(root, BACKUP_FILE)

    if not os.path.exists(bkPath):
            log("The path for the backup file does not exists: " + bkPath)
            return

    try:
        bkFile = open(bkPath)
    except e:
        log("Failed to open backup path for reading: " + bkPath)
        return

    bk = pickle.load(bkFile)

    try:
        bkFile.close()
    except:
        pass

    for shortcut_location, original_info in bk.backup:
        if verbose: log(shortcut_location)
        try:
            set_shortcut_info(shortcut_location, original_info)
        except:
            log("Failed to restore " + shortcut_location)

    log("")

    try:
        os.remove(bkPath)
    except:
        log("Error whilst deleting backup file")

    log("Restored state from " + bkPath)
