#!/usr/bin/env python

from ss_commmon import *

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

    try:
        os.remove(bkPath)
    except:
        log("Error whilst deleting backup file")

    log("Restored state from " + bkPath)


def do_revert(file, bk):
    pass


def itertree(path, depth_limit, verbose, bk, depth=1):
    counter = 0
    
    for root, dirs, files in os.walk(path):
        if verbose: log("--- " + root + " ---")
        
        for f in files:
            if f.endswith('.lnk'):
                do_revert(f, bk)
                counter += 1
                if verbose: log(f)
        
        if depth < depth_limit:
            for dir in dirs:
                counter += itertree(dir, depth_limit, verbose, depth + 1)
        
    return counter
