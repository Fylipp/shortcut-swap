#!/usr/bin/env python

from ss_common import *

import os
import pickle


def swap(root, link, depth_limit=12, verbose=False):
        if not os.path.exists(root):
            log("The provided path does not exist")
            return

        dump = RevertDump(depth_limit)

        bkPath = os.path.join(root, BACKUP_FILE)

        if os.path.exists(bkPath):
            log("The path for the backup file already exists: " + bkPath)
            return

        try:
            bkFile = open(bkPath, 'w')
        except e:
            log("Failed to open backup path for writing: " + bkPath)
            return
        
        counter = itertree(root, depth_limit, verbose, dump)
        
        if counter is 0:
            log("No files affected.")
        elif counter is 1:
            log("1 file affected.")
        else:
            log(str(counter) + " files affected.")

        pickle.dump(dump, bkFile)

        try:
            bkFile.close()
        except:
            pass

        log("Backup is located at " + bkPath)


def do_swap(file, bk):
    pass

    
def itertree(path, depth_limit, verbose, bk, depth=1):
    counter = 0
    
    for root, dirs, files in os.walk(path):
        if verbose: log("--- " + root + " ---")
        
        for f in files:
            if f.endswith('.lnk'):
                do_swap(f, bk)
                counter += 1
                if verbose: log(f)
        
        if depth < depth_limit:
            for dir in dirs:
                counter += itertree(dir, depth_limit, verbose, depth + 1)
        
    return counter
