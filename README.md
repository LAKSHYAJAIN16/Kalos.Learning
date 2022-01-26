# Kalos.Learning
A Machine Learning Framework written in pure c#

## But..Why?
C# already has `ML.Net`, why would you need another C# ML Framework?\
ML.Net is a complex tool which is best for more complex projects. But if you just want to get your hands dirty and make a simple sentiment analysis project, you're gonna spend half your time watching tutorials and installing the **trillion dependencies** ML.Net has.

## Features
- Includes Popular Machine Learning Algorithms like Linear Regression, SVM, Decision Trees and much more.

- The Framework is scalable and users can create their own models with the specified Interfaces.

- Is Very thorough and performant.Beats Python's performance EVERYDAY OF THE WEEK.

- Comes with example datasets and tests.

## Examples
### SVM
![svm](https://raw.githubusercontent.com/LAKSHYAJAIN16/assets/main/snap2.png)

### Decision tree
![dt](https://raw.githubusercontent.com/LAKSHYAJAIN16/assets/main/snap3.png)

### Perceptron
![perceptron](https://raw.githubusercontent.com/LAKSHYAJAIN16/assets/main/snap4.png)

### Naive Bayes
![nb](https://raw.githubusercontent.com/LAKSHYAJAIN16/assets/main/snap5.png)

### Logistic Regression
![lr](https://raw.githubusercontent.com/LAKSHYAJAIN16/assets/main/snap6.png)

And that's scratching the surface.

## Neural Network
The Main Focus of `Kalos.Learning` is Neural Networks. We offer a **scalable, expandable and easy to use** way to implement Neural Networks, comprised of 4 elements :-

- **Layers** : We offer 5 layer-types : `Dense`, `Convo2D`, etc. They should get you started, but if you need more layers you can create your own by building off the `KLayer` Interface.

- **Activation Functions** : We offer 6 activation functions : `Sigmoid`, `ReLu`, etc. They should get you started, but if you need more functions you can create your own by building off the `KActivationLayer` Interface.

- **Models** : We currently only have one Model Type : the `Sequential`, because that is the most widely used one, but we are trying to add more model types. If you want to create your own ModelType, you can build off the `KModel` Interface.

- **Input Functions** : Wait...what? Yes, with Kalos.Learning, you can create your own input functions. Normally, in frameworks like Tensorflow this part is abstracted and hided, but with `Kalos.Learning`, you can **create your own neuron input function**. We have 3 builtin functions, like `Sum`, `WeightedSum`, etc, but you can create your own by building off the `KInputLayer` Interface.

`Kalos.Learning` Neural Networks are more of a 'Choose your Adventure' type story. We value **flexibility** and allow the user to create whatever layer, model or function.

## Utilities
- CSV and Dat file Parser (to import datasets)
- Preprossesing Module (to format the data)
- Debugging Linq Tools (2D array printing, dynamic printing, etc)

## Graphs
We also have our very own Graphing Module, which you can access in `Kalos.Leaning.Linq.Graphs` Namespace with the `GraphService` Class. We currently only support Line Graphs.

## Summary
With `Kalos.Learning`, you get a **flexible, powerful and simple** way to implement machine learning in your C# project, with **minimal configuration or dependencies**.