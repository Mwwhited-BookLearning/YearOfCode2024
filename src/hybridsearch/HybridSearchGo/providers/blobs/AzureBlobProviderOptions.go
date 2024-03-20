package blobs

type AzureBlobProviderOptions struct {
	ConnectionString       string
	DocumentCollectionName string
	SummaryCollectionName  string
}

func CreateOptions() AzureBlobProviderOptions {
	return AzureBlobProviderOptions{
		ConnectionString:       "DefaultEndpointsProtocol=https;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://192.168.1.170:10000/devstoreaccount1;",
		DocumentCollectionName: "docs",
		SummaryCollectionName:  "summary",
	}
}
