#!/bin/bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd $DIR/../..
GITROOT=`pwd`
cd $GITROOT/Utilities/cfastbot
nohup ./run_cfastbot.sh &
tail  -f nohup.out
