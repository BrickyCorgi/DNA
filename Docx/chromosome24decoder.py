def check_chromosome24(a):
    
    dna = open(a)
    total = 0
    empty = 0
    
    for line in dna:
        if line.startswith("#"):
            continue
        dna_data = line.split()
        if dna_data[1] == '24':
            total += 1
            if dna_data[3] == '0':
                empty += 1    
    print(empty, total, sep="/")
                

