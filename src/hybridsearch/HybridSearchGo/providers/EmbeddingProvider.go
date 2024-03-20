package providers

type EmbeddingProvider interface {
	Embed(text string) ([]float32, error)
	Length() int
}
