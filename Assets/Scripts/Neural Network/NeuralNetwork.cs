using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class NeuralNetwork : MonoBehaviour , IComparable<NeuralNetwork>
{
    private int[] m_Layers;
    private float[][] m_Neurons;
    private float[][][] m_Weights;
    private float m_Fitness;

    public NeuralNetwork(int[] layers)
    {
        this.m_Layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            this.m_Layers[i] = layers[i];
        }
        //Inicializar matrizes
        InitNeurons();
        InitWeights();
    }

    public NeuralNetwork(NeuralNetwork copyNetwork)
    {
        this.m_Layers = new int[copyNetwork.m_Layers.Length];
        for (int i = 0; i < copyNetwork.m_Layers.Length; i++)
        {
            this.m_Layers[i] = copyNetwork.m_Layers[i];
        }
        InitNeurons();
        InitWeights();
        CopyWeights(copyNetwork.m_Weights);
    }
    private void CopyWeights(float[][][] copyWeights)
    {
        for (int i = 0; i < m_Weights.Length; i++)
        {
            for (int j = 0; j < m_Weights[i].Length; j++)
            {
                for (int k = 0; k < m_Weights[i][j].Length; k++)
                {
                    m_Weights[i][j][k] = copyWeights[i][j][k];
                }
            }
        }
    }

    private void InitNeurons() //Cria a matriz de neuronios
    {
        //Inicialização do neuronio
        List<float[]> neuronsList = new List<float[]>();
        for (int i = 0; i < m_Layers.Length; i++) 
        {
            neuronsList.Add(new float[m_Layers[i]]); //Adiciona layer na lista de neuronios
        }
        m_Neurons = neuronsList.ToArray();
    }

    private void InitWeights() //Cria a matriz de pesos
    {
        List<float[][]> weightsList = new List<float[][]>();
        
        for (int i = 1; i < m_Layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>(); 
            int neuronsInPreviousLayer = m_Layers[i - 1];
            
            for (int j = 0; j < m_Neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer]; 
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    //Adiciona pesos aleatorios
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f,0.5f);
                }
                layerWeightsList.Add(neuronWeights); //Adiciona pesos dos neuronios aos pesos das da camada
            }
            weightsList.Add(layerWeightsList.ToArray());
        }
        m_Weights = weightsList.ToArray();
    }

    public float[] FeedForward(float[] inputs)//Inputs para a rede neural
    {
        //Adiciona os inputs a matriz de neuronios
        for (int i = 0; i < inputs.Length; i++)
        {
            m_Neurons[0][i] = inputs[i];
        }
        
        for (int i = 1; i < m_Layers.Length; i++)
        {
            for (int j = 0; j < m_Neurons[i].Length; j++)
            {
                float value = 0f;
                for (int k = 0; k < m_Neurons[i-1].Length; k++)
                {
                    value += m_Weights[i - 1][j][k] * m_Neurons[i - 1][k]; //soma de todos os pesos das sinapses desse neurônio com os valores da camanda anterior
                }
                m_Neurons[i][j] = (float)Math.Tanh(value); //Hyperbolic tangent activation
            }
        }
        return m_Neurons[m_Neurons.Length-1]; //Retorna a output layer
    }
  
    public void Mutate()//Mutação dos pesos da rede neural
    {
        for (int i = 0; i < m_Weights.Length; i++)
        {
            for (int j = 0; j < m_Weights[i].Length; j++)
            {
                for (int k = 0; k < m_Weights[i][j].Length; k++)
                {
                    float weight = m_Weights[i][j][k];
                    //Mutação - valor do peso
                    float randomNumber = UnityEngine.Random.Range(0f,100f);
                    if (randomNumber <= 2f)
                    {
                      //Troca o sinal do weight
                        weight *= -1f;
                    }
                    else if (randomNumber <= 4f)
                    {
                      //Define um peso aleatorio entre -1 e 1
                        weight = UnityEngine.Random.Range(-0.5f, 0.5f);
                    }
                    else if (randomNumber <= 6f)
                    { 
                      //Aumento aleatorio entre0% e 100%
                        float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
                        weight *= factor;
                    }
                    else if (randomNumber <= 8f)
                    {
                      //Decremento aleatorio entre 0% e 100%
                        float factor = UnityEngine.Random.Range(0f, 1f);
                        weight *= factor;
                    }
                    m_Weights[i][j][k] = weight;
                }
            }
        }
    }
    public void AddFitness(float fit)
    {
        m_Fitness += fit;
    }
    public void SetFitness(float fit)
    {
        m_Fitness = fit;
    }
    public float GetFitness()
    {
        return m_Fitness;
    }

    public int CompareTo(NeuralNetwork other)// Compara duas redes neurais e escolhe uma baseada no fitness
    {
        if (other == null) return 1;
        if (m_Fitness > other.m_Fitness)
            return 1;
        else if (m_Fitness < other.m_Fitness)
            return -1;
        else
            return 0;
    }
}
