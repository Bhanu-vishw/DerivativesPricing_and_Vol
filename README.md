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

## volatility_transformers.ipynb

Developed a hybrid volatility forecasting framework that combines Transformer-based deep learning with GARCH statistical modeling to predict market volatility. Implemented a calibrated ensemble approach using non-constrained optimization to merge the pattern-recognition strengths of neural networks with the statistical rigor of GARCH, significantly reducing forecasting error (RMSE) and correcting for model-specific biases.

Methods / Models:
Transformer Neural Networks (Attention Mechanism), GARCH(1,1) Modeling, Time-Series Forecasting, Ensemble Learning, Linear Calibration, Scipy Optimization (L-BFGS-B).

Libraries:
PyTorch, arch, yfinance, scikit-learn, scipy, pandas, numpy, matplotlib

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


## IndexCorrelation_Final.ipynb

Analyzed correlations across equity indices to understand co-movement patterns and diversification effects within global equity markets. Evaluated time-varying correlation structures and their implications for portfolio construction and risk management.

Methods / Models:
Correlation analysis, covariance estimation, portfolio diversification analysis, statistical time-series analysis.

Libraries:
pandas, numpy, scipy, matplotlib, seaborn

## DeltaHedged_Option_QuantLib.ipynb

Implemented a delta-hedged option trading strategy to analyze hedging effectiveness and replication of option payoffs. Simulated dynamic hedging by adjusting underlying asset positions and evaluated hedging performance over time.

Methods / Models:
Black–Scholes option pricing, delta hedging, dynamic hedging strategies, option replication.

Libraries:
QuantLib, pandas, numpy, matplotlib

