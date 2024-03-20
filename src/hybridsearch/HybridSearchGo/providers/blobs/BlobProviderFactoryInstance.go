package blobs

import (
	"github.com/Azure/azure-sdk-for-go/sdk/storage/azblob"
	"hybrid-search/webapi/providers"
)

type BlobProviderFactoryInstance struct {
}

func CreateBlobProviderFactory() BlobProviderFactory {
	return BlobProviderFactoryInstance{}
}

func (factor BlobProviderFactoryInstance) Create(client *azblob.Client, collectionName string) providers.BlobProvider {
	return BlobProviderInstance{
		Client:         client,
		CollectionName: collectionName,
	}
}
