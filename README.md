# Wox-UnitConverter

A simple but powerfull physical units converter for Wox.

* **Home** : https://github.com/gissehel/Wox-UnitConverter
* **Keyword** : `unit`

# Usage by example


## Simple usage

```
unit 2cm
``` 

![(unit 2cm)](doc/capture-01-2cm.png)


``` 
unit 50 g
``` 

![(unit 50 g)](doc/capture-01-50g.png)

Note that as weird as is may be, standard SI unit for mass is kilogram and not gram.

## Composite unit

``` 
unit 2cm/s
``` 

![(unit 2cm/s)](doc/capture-02-2cm_by_s.png)

``` 
unit 2cm/h
``` 

![(unit 2cm/h)](doc/capture-02-2cm_by_h.png)

## Conversion between SI and US unit systems

``` 
unit 70 kg -> lbm
``` 

![(unit 70 kg -> lbm)](doc/capture-03-kg_to_lbm.png)

## Support for derivative SI units

``` 
unit 70 N
``` 

![(unit 70 N)](doc/capture-04-70_N.png)

``` 
unit 70 N -> Pa.m2
``` 

![(unit 70 N -> Pa.m2)](doc/capture-04-70_N_to_Pa_m2.png)

``` 
unit 1 MV/kA -> cΩ
``` 

![(unit 1 MV/kA -> cΩ)](doc/capture-04-megavolt_by_kiloampere_to_centiohm.png)

## Support for computer related units

``` 
unit 1 B -> bit
``` 

![(unit 1 B -> bit)](doc/capture-05-B_to_bit.png)


## Support for computer related prefix

``` 
unit 1 KiB -> kB
``` 

![(unit 1 KiB -> kB)](doc/capture-05-KiB_to_kB.png)

``` 
unit 1 kB -> Kibit
``` 

![(unit 1 kB -> Kibit)](doc/capture-05-kB_to_Kibit.png)

## Support for time related units

``` 
unit 1 min
``` 

![(unit 1 min)](doc/capture-06-min.png)


``` 
unit 1 h
``` 

![(unit 1 h)](doc/capture-06-h.png)


``` 
unit 1 h -> min
``` 

![(unit 1 h -> min)](doc/capture-06-h_to_min.png)

``` 
unit 1 d -> min
``` 

![(unit 1 d -> min)](doc/capture-06-d_to_min.png)

``` 
unit 1 y
``` 

![(unit 1 y)](doc/capture-06-y.png)


``` 
unit 1 y -> d
``` 

![(unit 1 y -> d)](doc/capture-06-y_to_d.png)

## Support for time related units with prefix from weird to weirder

``` 
unit 60 y -> kd
``` 

![(unit 60 y -> kd)](doc/capture-06-y_to_kd.png)

``` 
unit 60 y -> Kid
``` 

![(unit 60 y -> Kid)](doc/capture-06-y_to_Kid.png)

## Support for conversion to not only units but also value

``` 
unit 8 m -> 30 cm
``` 

![(unit 8 m -> 30 cm)](doc/capture-07-8m_to_30cm.png)

There is about **26.6...** times **30 cm** in **8 m**.

## Support for conversion to not homogenous units

This may seems weird, but it's in fact usefull.

``` 
unit 8 m2 -> 30 cm
``` 

![(unit 8 m2 -> 30 cm)](doc/capture-07-8m2_to_30cm.png)

Converting **8 m<sup>2</sup>** to **30 cm** doesn't make any sense ? Well, in fact, if you want to convert **8 m<sup>2</sup>** to **30 cm**, you'll need to take **26.66.. m** long of your **30 cm** large to make **8 m<sup>2</sup>**.

It does make sense after all. In fact, it's function that is the most usefull in this plugin.

``` 
unit 0.2 cm -> in.min-1
``` 

![(unit 0.2 cm -> in.min-1)](doc/capture-07-cm_to_in_by_min.png)

It will take **4.72 s** with a speed of **1 in by minute** to make **0.2 cm**.


``` 
unit 0.2 cm -> 3.1 in.min-1
``` 

![(unit 0.2 cm -> 3.1 in.min-1)](doc/capture-07-cm_to_3.1in_by_min.png)

It will take **1.52 s** with a speed of **3.1 in by minute** to make **0.2 cm**.

## Real life computer/mobile speed example

You have a quota of **50 GB** of data with your mobile phone contract this month.
Your phone can download up to **60 Mibit/s**.

How long will you be able to last by downloading full speed ?

``` 
unit 50 GB -> 60 Mibit/s
``` 

![(unit 50 GB -> 60 Mibit/s)](doc/capture-08-50_GB_to_60_Mibit_by_s.png)

**6357.829 s** ! Doesn't make any sense to me. Let's express that in hour !

``` 
unit 50 GB -> 60 Mibit/s : h
``` 

![(unit 50 GB -> 60 Mibit/s : h)](doc/capture-08-50_GB_to_60_Mibit_by_s_h.png)

The answer is **1.766 hour**.

You'll expire you quota in less than 2 hours by downloading full speed.


Now let's take a slower speed of **2 Mibit/s**. How long will you last in days ?

``` 
unit 50 GB -> 2 Mibit/s : d
``` 

![(unit 50 GB -> 2 Mibit/s : d)](doc/capture-08-50_GB_to_2_Mibit_by_s_d.png)

You'll last **2.2 days**.

## Real life car example (for non US)

How much is the distance **300 km** if you convert it to the speed of **120 km/h** ?

``` 
unit 300 km -> 120 km/h
``` 

![(unit 300 km -> 120 km/h)](doc/capture-09-300_km_to_120_km_by_h.png)

It's **9000s**. How many hours is that ?

``` 
unit 300 km -> 120 km/h : h
``` 

![(unit 300 km -> 120 km/h : h)](doc/capture-09-300_km_to_120_km_by_h_h.png)

It's **2.5 hours**.
