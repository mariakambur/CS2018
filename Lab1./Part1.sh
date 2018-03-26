#!/bin/bash
cat $1 | tr -d " \t\n\r" | tr -d '[:punct:]' > prepared.txt
var=$(wc -m < prepared.txt)
echo "$var"
sed 's/\(.\)/\1\n/g' prepared.txt | sort | uniq -ic > occs.txt
while read occ let; do
prob=$(echo $occ/$var | bc -l)
echo "$let  $prob"
echo "$let $prob">>"list.txt"
done < occs.txt
ent=0;
while read let prob; do
result=$(echo "$prob*(l($prob)/l(2))"|bc -l)
ent=$(echo $ent+$result | bc -l) 
done < list.txt 
pos=$(echo $ent*-1|bc -l)
echo "entropy = $pos"
qty=$(echo $pos*$var |bc -l)
byts=$(echo $qty/8 | bc -l)
echo "quantity of information = $byts bytes"
rm list.txt