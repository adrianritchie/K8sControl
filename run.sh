kubectl apply -f test.yaml

kubectl scale --replicas=0 deployment/test
kubectl scale --replicas=1 deployment/test

kubectl wait --for=condition=Ready pod -l app=test

sleep 3

kubectl port-forward deployment/test 8085:80