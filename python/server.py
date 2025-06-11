import grpc
from concurrent import futures
import time
import os
import price_pb2_grpc as pb2_grpc
import price_pb2 as pb2

class PricingService(pb2_grpc.PricerServicer):
    def __init__(self, *args, **kwargs):
        pass

    def GetPrice(self, request, context):
        print(request)
        if "ford" in request.make.lower():
            price = 125000
            currencyCode = 'HUF'
        elif "brown" in request.color.lower():
             price = 15000
             currencyCode = 'SEK'
        elif "delorean" in request.model.lower():
             price = 75000
             currencyCode = 'USD'
        else:
            price = 1000 + (request.year * 10)
            currencyCode = 'EUR'
        return pb2.PriceReply(price=price,currencyCode=currencyCode)

def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    pb2_grpc.add_PricerServicer_to_server(PricingService(), server)
    server.add_insecure_port('[::]:5002')

    # If you've created a localhost HTTPS certificate, uncomment these lines to use it.
    # key = open('localhost.key', 'rb').read()
    # crt = open('localhost.crt', 'rb').read()
    # server_credentials = grpc.ssl_server_credentials(((key, crt,),))
    # server.add_secure_port('[::]:5003', server_credentials)

    server.start()
    print("Autobarn gRPC Pricing Server running.")
    server.wait_for_termination()

if __name__ == '__main__':
    serve()