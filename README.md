# Quantitative Options Pricing and Volatility Modeling

This repository implements several quantitative finance models for option pricing and volatility surface modeling. The project explores classical option pricing frameworks, numerical methods, and stochastic simulation techniques used in derivatives pricing and risk management.

The repository includes implementations of the Black–Scholes model, Cox–Ross–Rubinstein binomial tree, Monte Carlo simulations with variance reduction, and volatility smile modeling using the SVI (Stochastic Volatility Inspired) parameterization proposed by Jim Gatheral.

# Models Implemented

Black–Scholes model, Cox–Ross–Rubinstein binomial tree, Monte Carlo simulation, antithetic variates variance reduction, Van der Corput quasi-random sequences, SVI volatility smile parameterization.

# Techniques Demonstrated

option pricing, volatility modeling, numerical methods for derivatives pricing, Monte Carlo simulation, variance reduction methods, volatility smile calibration, stochastic modeling.

# Repository Structure

## Volatility_Smile_SVI_Gatheral.ipynb

Implementation of SVI volatility smile modeling based on the parameterization introduced by Jim Gatheral.

Concepts implemented

volatility smile fitting, SVI parameterization, nonlinear optimization, implied volatility surface smoothing.

Applications

volatility surface calibration, option pricing model consistency.

Libraries

numpy, scipy, pandas, matplotlib.

## Blackscholes.ipynb

Implementation of the Black–Scholes option pricing model for European options.

Concepts implemented

Black–Scholes formula, implied volatility, option Greeks, analytical pricing of European options.

Libraries

numpy, scipy, matplotlib, pandas.

## CRR_BinomialTree.ipynb

Implementation of the Cox–Ross–Rubinstein (CRR) binomial tree model for option pricing.

Concepts implemented

Binomial option pricing, risk-neutral probability, backward induction pricing, lattice-based valuation of derivatives.

Libraries

numpy, pandas, matplotlib.

## MC_Simulation.cs

Monte Carlo simulation engine for pricing derivatives under stochastic processes.

Concepts implemented

Monte Carlo pricing, stochastic simulation, path generation, option payoff estimation.

Techniques

variance reduction, statistical convergence of simulations.

## MC_Antithetic_VDC.cs

Extension of the Monte Carlo engine using variance reduction techniques.

Concepts implemented

Antithetic variates, variance reduction in Monte Carlo simulation, quasi-random sampling using Van der Corput sequences.

## Random_Normal_Generator.cs

Custom implementation of normal random number generation for simulation frameworks.

Concepts implemented

pseudo-random number generation, Gaussian sampling, statistical simulation inputs.
