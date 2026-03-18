


# Mesh Test Definitions

The mesh tests are designed to identify systematic errors in the numerics of the algorithms, e.g. sinusodial waves, or systematic degradation towards the rims of the time interval.


# 1 mesh definitions inside the specification interval

## 1.1 Deterministic mesh 

The density of the mesh shall be defined as follows:
1) over the entire time range 10 years. We determine the coordinates every year on Jan 1st
2) for the 100 years towards the rim of the interval [1600, 2500] the density shall be 5 years: 
a) [1600...1700]
b) [2400...2500]
3) additionally for the 10 years at the rim of the interval [1600, 2500 ]the density shall be one year
a) [1600...1610]
b) [2490...2500]

##  1.2 Random mesh

There shall be 1000 random test points inside the mesh.

# 2 mesh definitions outside the specification interval

It appears not reasonable to test data outside the specified range. Nevertheless it makes sense to evaluate the degradation of the accuracy outside the specification interval. Be it for academic purposes or even to give ballpark figures for the user when working outside the interval.

## 1.1 Deterministic mesh 

3) for the 10 years right outside  border the interval [1600, 2500 ]the density shall be one year
a) [1600...1610]
b) [2490...2500]



