#!/usr/bin/env python

import os, sys
import pickle
from fire.core import Fire


class ShortcutSwap:
    def swap(self, root, link, backup=True, depth_limit=12, verbose=False):
        if not backup:
            if not assert_backup_disabled():
                return

        dump = RevertDump() if backup else None
        
        counter = itertree(root, depth_limit, verbose, dump)
        
        if counter is 0:
            log("No files affected.")
        elif counter is 1:
            log("1 file affected.")
        else:
            log(str(counter) + " files affected.")
        
        pass

    def revert(self, root):
        pass


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

    
def assert_backup_disabled():
    log("The backup feature has been disabled, this can cause permanent damage which may be fatal to the system!")
    sure = input("Are you sure you want to proceed [y/N]: ").strip().lower()
    if sure == 'y':
        log("Continuing with the backup feature disabled.")
        return True
    else:
        log("Operation aborted.")
        return False
        

def log(line, end='\n'):
    sys.stdout.write(line if (end == '') else (line + end))


class RevertDump:
    def __init__(self, format_version=1):
        self.format_version = format_version
        self.backup = dict()


def main():
    Fire(ShortcutSwap)

if __name__ == '__main__':
    main()
