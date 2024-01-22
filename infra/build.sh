#!/bin/bash
set -euo pipefail

echo "Sourcing environment defaults..."
source ../env.sh

echo "Synthesizing infrastructure..." 
cdk synth