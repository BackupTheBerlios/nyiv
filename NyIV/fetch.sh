#!/bin/bash

LOGIN_NAME="matteo_bertozzi"

export CVS_RSH=ssh

echo " * Fetch Niry-Sharp"
cvs -z3 -d${LOGIN_NAME}@cvs.niry-sharp.berlios.de:/cvsroot/niry-sharp co Niry-Sharp

echo " * Fetch NyIV"
cvs -z3 -d${LOGIN_NAME}@cvs.nyiv.berlios.de:/cvsroot/nyiv co Niry-Sharp

echo " * Fetch NyFolder"
cvs -z3 -d${LOGIN_NAME}@cvs.nyfolder.berlios.de:/cvsroot/nyfolder co NyFolder

cvs -dmatteo_bertozzi@cvs.berlios.de:/cvsroot/nyiv import "NyIV" "NyIV" start
cvs -dmatteo_bertozzi@cvs.berlios.de:/cvsroot/niry-sharp import "niry-sharp" "niry-sharp" start
