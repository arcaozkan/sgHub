import numpy as np
from math import *
from matplotlib import pyplot as plt


# This Library works best for y scores between ±1
# Because it is based on normalized gaussian process
# Needs to be updated for experiments with broader range of scores

# GP Functions work with multi-parameters (such as x=[x1,x2])
# BO example given for only 1D

#----------- Gaussian Distribution-------------------
def CDF(x,mu,sigmasqr):
    """
    :param x:
    :param mu:
    :param sigmasqr:
    :return: CDF(x, Mu=mu,Sigma=Sqrt(sigmasquare))
    """
    if round(sigmasqr,5) !=0:
        CumDistFunct=0.5*(1+erf((x-mu)/sqrt(2*sigmasqr)))
    else:
        CumDistFunct=0
    return CumDistFunct

def PDF(x,mu,sigmasqr):
    """
    :param x:
    :param mu:
    :param sigmasqr:
    :return: PDF(x, Mu=mu,Sigma=Sqrt(sigmasquare))
    """
    if round(sigmasqr,5) !=0:
        sigma=sqrt(abs(sigmasqr))
        PDF=(1/(sigma*sqrt(np.pi)))*exp(-0.5*((x-mu)/sigma)**2)
    else:
        PDF=0
    return PDF

# -------------Gaussian Process Part-----------------
def rbf_kernel(x1,x2,theta):
    """
    :param x1:
    :param x2:
    :param theta: (needs to be optimized)
    :return:

    RBF Kernel       => k(x1,x2) = e^(-theta*(x1-x2)^2
    """
    return exp(-theta*(np.linalg.norm(x1-x2)**2))

def Kernel_Matrix(xpoints,theta,noise=0.0):
    """
    :param xpoints: sampled x points
    :param theta:
    n: len(xpoints)
    Kernel: Kernel Matrix,                                  size ( n x n)
    -------------------------
    Kernel Matrix:[K],

    [K]=[[K(x1,x2)..K(x1,xn)]
         .
         .
         [K(xn,x1)..K(xn,xn)]]
    --------------------------

    :return: Kernel Matrix
    """
    n=len(xpoints)
    Kernel=np.zeros((n,n)) # creating Kernel matrix
    Measurement_noise=np.identity(n)*noise # adding Measurement noise (optional)

    # assigning Kernel function to empty matrix
    for i in range(n):
        for j in range(n):
            Kernel[i,j]=rbf_kernel(xpoints[i],xpoints[j],theta)
    # creating ones
    Kernel = Kernel + Measurement_noise

    return Kernel

def correlation_vector(xstar, xarr,theta):
    """
    correlation vector between unknown xpoint(xstar) and every known x points(xarr)
    correlation vector: [ K(x*,x1),K(x*,x2)..K(x*,xn)]
    :param xstar: x* where f(x*) is unknown                     scalar
    :param xarr: array of x points where f(x) is known          array
    :param theta: parameter used in gaussian kernel             scalar

    :return: correlation_vect                                   array
    """
    n=len(xarr)
    corr_vect = np.zeros(n)

    for i in range(n):
        corr_vect[i] = rbf_kernel(xstar, xarr[i],theta)
    return corr_vect

def Gaussian_Process(xarr,xpoints,ypoints,theta,noise=0.01):

    K=Kernel_Matrix(xpoints,theta,noise=noise)
    invK=np.linalg.inv(K)

    # mu_s = k_sx@((K+noise*I)^-1)y_meas
    # ss_s = k_ss+noise - k_sx@((K+noise*I)^-1)@k_xs
    ##  Predicting mean & variance
    mu_ss=np.zeros(len(xarr))
    ss_s=np.zeros(len(xarr))

    for i in range(len(xarr)):
        ksx=correlation_vector(xarr[i],xpoints,theta)
        mu_ss[i]= ksx.T @ invK @ ypoints
        ss_s[i] = (1) - (ksx.T @ invK @ ksx)

    return(mu_ss,ss_s)

# --------------- Bayesian Optimization ---------------
def acquisitionFunct(xarr,ypoints,mupred,sigmaSqrPred,Funct="UCB"):
    """
    :param xarr: x domain array
    :param ypoints: measured y points y=f(x)
    :param mupred: predicted mean (Mu*) values for f(x) according to gaussian process
    :param sigmaSqrPred: predicted variance (sigma^2)* values for f(x) according to gaussian process

    M E T H O D S
    ----------------------------------------------
    Expected Improvement(EI):

    Z=(Mpred-Fmax-c*Fmax)/SigmaSqrPred  --> Z score

    EI(x)= (Mpred-fmax-c*Fmax) * CDF(Z,Mu=0,Ssqr=1) + sigmaSqrPred * PDF(Z,Mu=0,Ssqr=1)

    *-*-*-*-*-*-*-*
    Upper Boundary (UB): Max point must be inside of Mu_posterior+2sigma_posterior   (%95)

    UB(x)=Mu_p(x)+2*sigma_p(x)-fmax  if Mu_p(x)+2*sigma_p(x)-fmax  > 0   else: 0

    ----------------------------------------------
    CDF(P(Mpred>Fmax)=1-CDF(Fmax>Mpred)

    :return:
    bestx: best x value to sample according to acquisation function results
    AcqArr: Array of Acquisation function results
    """
    AcqArr=np.zeros(len(xarr))
    ymax=ypoints[np.argmax(ypoints)]
    sigmapred = sigmaSqrPred


    if Funct=="EI":
        for i in range(len(xarr)):

            if np.round(sigmaSqrPred[i],6)>0:

                Z=(mupred[i]-ymax)/sigmapred[i] # Z score

                exploit=(mupred[i]-ymax)*CDF(Z,0,1) # Exploit part
                explore=sigmapred[i]*PDF(Z,0,1)     # Explore Part
                EI=exploit+explore                  # Acq Funct score

                # acqArr[i]=max(0,EI)
                if np.round(EI,6)>0:

                    AcqArr[i]=EI
                else:
                    AcqArr[i] = 0

            else:
                AcqArr[i]=0

        bestx=xarr[np.argmax(AcqArr)]

        return(bestx,AcqArr)

    if Funct=="UCB":
        for i in range(len(xarr)):
            AcqArr[i]=mupred[i]+2*sigmapred[i]

        bestx=xarr[np.argmax(AcqArr)]

        return(bestx,AcqArr)


# -Optimization Example
def SimulateBo_1D(x_bound,Opt_func,theta,noise=0.0,iteration=10,plot_result=True):

    x_init=x_bound[1]-x_bound[0]
    y_init=Opt_func(x_init)

    x_arr=np.linspace(x_bound[0],x_bound[1],200)

    #-------Optimization Part---------------
    x_meas=np.array([x_init])
    y_meas=np.array([y_init])
    mu_post=[]
    var_post=[]

    for i in range(iteration):
        mu_post,var_post=Gaussian_Process(x_arr,x_meas,y_meas,theta,noise=noise)

        x_sample,Acq_arr =acquisitionFunct(x_arr,y_meas,mu_post,var_post)

        y_sample=Opt_func(x_sample) # measure from real function
        x_meas=np.append(x_meas,x_sample)
        y_meas=np.append(y_meas,y_sample)
        print("iteration: "+str(i)+"\n"+10*"-*")
        print("sampled x point: "+str(x_sample))
        print("best measured x point: "+str(x_meas[np.argmax(y_meas)]))

    #----Comparing with Real Function-----
    y_real=np.zeros(len(x_arr))

    for i in range(len(x_arr)):
        y_real[i]=Opt_func(x_arr[i])

    if plot_result is True:
        plt.fill_between(x_arr, mu_post + var_post, mu_post - var_post, alpha=0.5)
        plt.plot(x_arr, mu_post)
        plt.plot(x_meas, y_meas, 'o', alpha=0.4)
        plt.plot(x_arr,y_real,'--')
        plt.show()
    return x_meas,y_meas,mu_post,var_post












