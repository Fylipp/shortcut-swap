#!/usr/bin/env python

from ss_common import *

import os
import pickle


def swap(root, link, depth_limit=2, verbose=False):
        if not os.path.exists(root):
            log("The provided path does not exist")
            return

        dump = RevertDump()

        bkPath = os.path.join(root, BACKUP_FILE)

        if os.path.exists(bkPath):
            log("The path for the backup file already exists: " + bkPath)
            return

        try:
            bkFile = open(bkPath, 'w')
        except e:
            log("Failed to open backup path for writing: " + bkPath)
            return
        
        counter = itertree(root, link, depth_limit, verbose, dump)
        
        log("")

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


def do_swap(file, new_target, bk):
    old = get_shortcut_info(file)
    bk.backup[file] = old
    set_shortcut_info(file, ShortcutInfo(new_target), os.path.join(old.working_directory, old.target_path))

    
def itertree(path, new_target, depth_limit, verbose, bk):
    counter = 0
    depth = 0
    
    for root, dirs, files in os.walk(path):
        if verbose: log("--- " + root + " ---")

        depth += 1
        
        for f in files:
            if f.endswith('.lnk'):
                if verbose: log(f)
                try:
                    do_swap(f, new_target, bk)
                    counter += 1
                except Exception as e:
                    log("Failed to swap " + path + " with the following error: " + e.message)

        if depth >= depth_limit:
            break
    
    return counter
