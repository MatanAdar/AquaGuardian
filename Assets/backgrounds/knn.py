from collections import Counter
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from sklearn.model_selection import train_test_split

# Load data
file_path = 'haberman.data'
dataset = pd.read_csv(file_path)
dataset.columns = ['feature_1', 'feature_2', 'feature_3', 'label']

# Visualize data
fig = plt.figure()
ax = fig.add_subplot(111, projection='3d')
ax.scatter(dataset['feature_1'], dataset['feature_2'], dataset['feature_3'], c=dataset['label'])
ax.set_xlabel('Feature_1')
ax.set_ylabel('Feature_2')
ax.set_zlabel('Feature_3')
plt.show()

# Distance functions
def custom_manhattan_distance(v1, v2):
    return np.sum(np.abs(v1 - v2))

def custom_euclidean_distance(v1, v2):
    return np.sqrt(np.sum((v1 - v2) ** 2))

def custom_frechet_distance(v1, v2):
    return np.max(np.abs(v1 - v2))

# KNN Classifier
class CustomKNN:
    def __init__(self, k=1, distance_metric=None):
        self.k = k
        self.distance_metric = distance_metric

    def fit(self, X, y):
        self.X_train = X
        self.y_train = y

    def predict(self, X):
        predictions = [self._predict(x) for x in X]
        return np.array(predictions)

    def _predict(self, x):
        distances = [self.distance_metric(x, x_train) for x_train in self.X_train]
        k_indices = np.argsort(distances)[:self.k]
        k_nearest_labels = [self.y_train[i] for i in k_indices]
        most_common_label = Counter(k_nearest_labels).most_common(1)[0][0]
        return most_common_label

# Error computation
def calculate_classification_error(y_true, y_pred):
    return np.mean(y_true != y_pred)

# KNN experiment execution
def run_custom_knn_experiment(data, n_iterations, k_values, distance_metrics):
    X_data = data.iloc[:, :-1]
    y_data = data['label']

    empirical_errors = np.zeros((len(k_values), len(distance_metrics)))
    true_errors = np.zeros((len(k_values), len(distance_metrics)))

    for i, k in enumerate(k_values):
        for j, distance_metric in enumerate(distance_metrics):
            empirical_error_list = []
            true_error_list = []

            for iteration in range(n_iterations):
                X_train, X_test, y_train, y_test = train_test_split(X_data, y_data, test_size=0.5, random_state=iteration)
                custom_knn = CustomKNN(k=k, distance_metric=distance_metric)
                custom_knn.fit(X_train.to_numpy(), y_train.to_numpy())

                y_train_pred = custom_knn.predict(X_train.to_numpy())
                empirical_error_list.append(calculate_classification_error(y_train.to_numpy(), y_train_pred))

                y_test_pred = custom_knn.predict(X_test.to_numpy())
                true_error_list.append(calculate_classification_error(y_test.to_numpy(), y_test_pred))

            empirical_errors[i, j] = np.mean(empirical_error_list)
            true_errors[i, j] = np.mean(true_error_list)

    return empirical_errors, true_errors

# Results printing
def display_custom_knn_results(empirical_errors, true_errors, k_values, distance_metrics):
    print("Custom KNN Classification Results:")
    print("----------------------------------\n")

    for i, k in enumerate(k_values):
        print(f"---------------k={k}----------------")
        for j, distance_metric in enumerate(distance_metrics):
            print(f"*using {distance_metric.__name__}*:")
            print(f"  Average Empirical Error: {empirical_errors[i, j]:.3f}")
            print(f"  Average True Error: {true_errors[i, j]:.3f}\n")
# Results plotting
def visualize_custom_knn_results(empirical_errors, true_errors, k_values, distance_metrics):
    fig, ax = plt.subplots(1, 2, figsize=(14, 5))

    for j, distance_metric in enumerate(distance_metrics):
        ax[0].plot(k_values, empirical_errors[:, j], label=distance_metric.__name__)
        ax[1].plot(k_values, true_errors[:, j], label=distance_metric.__name__)

    for a in ax:
        a.set_xlabel('Number of Neighbors: k')
        a.grid(True)

    ax[0].set_title('Average Empirical Errors')
    ax[0].set_ylabel('Error')
    ax[0].legend()

    ax[1].set_title('Average True Errors')
    ax[1].set_ylabel('Error')
    ax[1].legend()

    plt.tight_layout()
    plt.show()

# Execution
custom_k_values = [1, 3, 5, 7, 9]
custom_distance_metrics = [custom_manhattan_distance, custom_euclidean_distance, custom_frechet_distance]
custom_empirical_errors, custom_true_errors = run_custom_knn_experiment(dataset, 100, custom_k_values, custom_distance_metrics)

# Print and plot results
display_custom_knn_results(custom_empirical_errors, custom_true_errors, custom_k_values, custom_distance_metrics)
visualize_custom_knn_results(custom_empirical_errors, custom_true_errors, custom_k_values, custom_distance_metrics)
