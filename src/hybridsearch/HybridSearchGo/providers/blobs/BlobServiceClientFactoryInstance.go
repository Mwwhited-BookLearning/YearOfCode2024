package blobs

import (
	"github.com/Azure/azure-sdk-for-go/sdk/storage/azblob"
)

type BlobServiceClientFactoryInstance struct {
}

func CreateClientFactory() BlobServiceClientFactoryInstance {
	return BlobServiceClientFactoryInstance{}
}

func (factory BlobServiceClientFactoryInstance) Create(options AzureBlobProviderOptions) *azblob.Client {
	client, err := azblob.NewClientFromConnectionString(options.ConnectionString, nil)
	if err != nil {
		return nil
	}
	return client
}
